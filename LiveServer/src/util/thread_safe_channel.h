//
// Created by SSAFY on 2024-04-30.
// 싱글톤 패턴을 사용하여 방 공유
//

#ifndef LIVESERVER_THREAD_SAFE_CHANNEL_H
#define LIVESERVER_THREAD_SAFE_CHANNEL_H

#include <boost/asio.hpp>
#include <set>
#include <unordered_map>

#include "shared_mutex.h"

enum {
    channel_number_max = 255
};

class ThreadSafeChannel {
public:
    static ThreadSafeChannel &getInstance() {
        static ThreadSafeChannel instance;
        return instance;
    }

    // 채널 번호 빈거 찾아서 채널 만들고 해당 채널 번호 리턴
    int makeInitChannel() {
        // std::lock_guard는 예외 발생 시 스코프 벗어나지 않아 unlock 되지 않으므로 unique_lock 사용
        std::unique_lock<std::mutex> lock(SharedMutex::getInstance().getMutex());

        for (int i = 0; i < channel_number_max; i++) {
            // 해당 채널 번호(i)가 존재하지 않으면 채널 번호 할당
            // 이미 존재하는 채널 번호라면 건너 뜀
            auto it = channels_.find(i);
            if (it == channels_.end()) {
                channels_[i];
                lock.unlock();
                return i;
            }
        }

        return -1;
    }

    // 입력받은 채널 넘버에 채널 만들기
    bool makeInitChannel(int channel_number) {
        // std::lock_guard는 예외 발생 시 스코프 벗어나지 않아 unlock 되지 않으므로 unique_lock 사용
        std::unique_lock<std::mutex> lock(SharedMutex::getInstance().getMutex());

        auto it = channels_.find(channel_number);
        // 해당 번호 채널이 존재하지 않을 경우 채널 만들기
        if(it == channels_.end()) {
            channels_[channel_number];
            lock.unlock();
            return true;
        }
        else {
            std::cout << "You're trying to make exist channel number " << channel_number << std::endl;
            return false;
        }
    }


//    // 채널 리스트에 유저 넣기(채널 번호 자동 할당 후 해당 채널 번호 리턴)
//    int insertUser(const boost::asio::ip::udp::endpoint &user_endpoint) {
//        // std::lock_guard는 예외 발생 시 스코프 벗어나지 않아 unlock되지 않으므로 unique_lock 사용
//        std::unique_lock<std::mutex> lock(SharedMutex::getInstance().getMutex());
//
//        for (int i = 0; i < channel_number_max; i++) {
//            // 해당 채널 번호(i)가 존재하지 않으면 채널 번호 할당
//            // 이미 존재하는 채널 번호라면 건너뜀
//            if (!channels_.count(i)) {
//                try {
//                    channels_[i].insert(user_endpoint);
//                } catch (...) {
//                    lock.unlock();
//                    throw;
//                }
//
//                return i;
//            }
//        }
//
//        return -1;
//    }

    // 채널 리스트에 유저 넣기(지정한 채널 번호로 넣기)
    void insertUser(const int channel_number, const boost::asio::ip::udp::endpoint &user_endpoint) {
//        std::lock_guard<std::mutex> lock(SharedMutex::getInstance().getMutex());
        std::unique_lock<std::mutex> lock(SharedMutex::getInstance().getMutex());

        auto it = channels_.find(channel_number);
        if(it != channels_.end()) {
            it ->second.insert(user_endpoint);
        }
        else {
            std::cout << "room is not exists" << std::endl;
        }
    }

    // 채널 번호로 같은 채널의 유저 엔드포인트 셋 찾기
    const std::set<boost::asio::ip::udp::endpoint> &getUserSet(const int channel_number) {
        std::lock_guard<std::mutex> lock(SharedMutex::getInstance().getMutex());
        static const std::set<boost::asio::ip::udp::endpoint> empty_set;

        auto it = channels_.find(channel_number);
        if(it != channels_.end()) {
            return it->second;
        }

        return empty_set;
    }

    // 해당 채널 번호 삭제
    void deleteChannel(const int channel_number) {
        std::lock_guard<std::mutex> lock(SharedMutex::getInstance().getMutex());
        if (channels_.count(channel_number)) { channels_.erase(channel_number); }
    }

    void printChannelUsers(const int channel_number) {
        std::lock_guard<std::mutex> lock(SharedMutex::getInstance().getMutex());

        std::set<boost::asio::ip::udp::endpoint> channel;
        auto it = channels_.find(channel_number);
        if(it != channels_.end()) {
        }

        std::cout << "channel user(" << it->second.size() << "): ";
        for (const boost::asio::ip::udp::endpoint &participant: it->second) {
            std::cout << participant << ", ";
        }
        std::cout << "\n";
    }

    void printExistChannelNumbers() {
        std::lock_guard<std::mutex> lock(SharedMutex::getInstance().getMutex());

        std::cout << "Exist channel numbers: ";
        for (int i = 0; i < channel_number_max; i++) {
            // 해당 채널 번호(i)가 존재하지 않으면 채널 번호 할당
            // 이미 존재하는 채널 번호라면 건너 뜀
            auto it = channels_.find(i);
            if (it != channels_.end()) {
                std::cout << i << ", ";
            }
        }

        std::cout << "\n";
    }

private:
    ThreadSafeChannel() = default;

    ThreadSafeChannel(const ThreadSafeChannel &) = delete;

    ThreadSafeChannel &operator=(const ThreadSafeChannel &) = delete;

    ThreadSafeChannel(ThreadSafeChannel &&) = delete;

    ThreadSafeChannel &operator=(ThreadSafeChannel &&) = delete;

    // 채널 목록(채널번호, 엔드포인트셋)
    std::unordered_map<int, std::set<boost::asio::ip::udp::endpoint>> channels_;
};

#endif// LIVESERVER_THREAD_SAFE_CHANNEL_H
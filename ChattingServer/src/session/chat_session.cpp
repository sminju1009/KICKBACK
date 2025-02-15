#include <functional>

#include "chat_session.h"

ChatSession::ChatSession(boost::asio::io_context &io_context, int channel_index)
        : socket_(io_context),
          channel_index_(channel_index),
          channel_(&ChannelList::get_instance().get_channel(channel_index)),
          receiveBuffer_() {
}

tcp::socket &ChatSession::socket() {
    return socket_;
}


void ChatSession::start() {
    channel_->join(shared_from_this());

    read_message();
}

void ChatSession::read_message() {
    memset(&receiveBuffer_, '\0', sizeof(receiveBuffer_));
    socket_.async_read_some(boost::asio::buffer(receiveBuffer_),
                            [this](const boost::system::error_code &error, size_t bytes_transferred) {
                                handle_read_message(error, bytes_transferred);
                            }
    );
}

void ChatSession::handle_read_message(const boost::system::error_code &error, size_t bytes_transferred) {
    if (error) {
        // 클라이언트의 엔드포인트 정보를 얻습니다.
        tcp::endpoint remote_ep = shared_from_this()->socket().remote_endpoint();
        // IP 주소와 포트 번호를 문자열로 변환합니다.
        std::string client_info = remote_ep.address().to_string() + ":" + std::to_string(remote_ep.port());

        std::cout << "Client leave: " << client_info << std::endl;

        std::cout << error << std::endl;

        channel_->leave(shared_from_this());
    } else {
        msgpack::sbuffer sbuf;
        msgpack::object_handle oh =
                msgpack::unpack(receiveBuffer_.data(), bytes_transferred);

        msgpack::object deserialized = oh.get();

        std::stringstream buffer;
        msgpack::pack(buffer, deserialized);

        std::pair<int, int> command = Message::command(deserialized);

        switch (command.first) {
            case 1:
                move_channel(command.second);
                break;
            case 2:
                move_channel(0);
                break;
            case 3:
                channel_->deliver(buffer.str());
                break;
            default:
                std::cout << "error" << std::endl;
                break;
        }

        read_message();
    }
}

int ChatSession::get_channel_index() const {
    return channel_index_;
}

void ChatSession::deliver(const std::string &msg) {
    bool write_in_progress = !write_msgs_.empty();
    write_msgs_.push_back(msg);
    if (!write_in_progress) {
        do_write();
    }
}

void ChatSession::do_write() {
    boost::asio::async_write(socket_,
                             boost::asio::buffer(write_msgs_.front().data(),
                                                 write_msgs_.front().length()),
                             [this](const boost::system::error_code &error, size_t bytes_transferred) {
                                 handle_write(error);
                             });
}

void ChatSession::handle_write(const boost::system::error_code &error) {
    if (!error) {
        write_msgs_.pop_front();
        if (!write_msgs_.empty()) {
            do_write();
        }
    } else {
        channel_->leave(shared_from_this());
    }
}

void ChatSession::move_channel(int new_channel_index) {
    Channel &new_channel = ChannelList::get_instance().get_channel(new_channel_index);

    new_channel.join(shared_from_this());

    channel_->leave(shared_from_this());

    channel_ = &new_channel;

    if (channel_index_ != 0 && ChannelList::get_instance().get_channel(channel_index_).participants_.empty()) {
        ChannelList::get_instance().remove_channel(channel_index_);
    }

    channel_index_ = new_channel_index;
}

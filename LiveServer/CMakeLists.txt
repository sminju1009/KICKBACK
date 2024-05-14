cmake_minimum_required(VERSION 3.11)
project(
        LiveServer
        VERSION 0.1
        DESCRIPTION "Boost.Asio 활용한 UDP 게임 서버"
        LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)

# Boost
find_package(Boost REQUIRED)
#Thread
find_package(Threads REQUIRED)

# 대상 추가
add_executable(${PROJECT_NAME}
        src/live_server.cpp
        src/udp/receiver.cpp
        src/udp/worker.cpp
        src/tcp/tcp_connect.cpp
        src/tcp/message_handler.h
        src/tcp/message_handler.cpp
        src/tcp/message_unit.h
        src/tcp/message_unit.cpp
        src/udp/model/connection_info_udp.h
        src/udp/model/message_form.h
        src/udp/util/shared_mutex.h
        src/udp/util/thread_safe_queue.h
        src/udp/util/thread_safe_queue.cpp
        src/udp/util/thread_safe_channel.h
        src/udp/util/thread_safe_channel.cpp
)
# 대상에 Boost 및 Thread 라이브러리 링크
target_link_libraries(${PROJECT_NAME} ${Boost_LIBRARIES} ${CMAKE_THREAD_LIBS_INIT})

# Boost, Msgpack
include_directories(${Boost_INCLUDE_DIRS} ${PROJECT_SOURCE_DIR}/include)

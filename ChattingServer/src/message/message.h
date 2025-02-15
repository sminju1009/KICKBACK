#pragma once

#ifndef CHATTING_SERVER_MESSAGE_H
#define CHATTING_SERVER_MESSAGE_H

#include <iostream>

#include "message_unit.h"

class Message {
    enum Command {
        LIVESERVER,
        CLIENT,
        CREATE,
        JOIN,
        LEAVE,
        READY,
        START,
        ITEM,
        END,
        CHAT,
        TEMP,
        MAP,
        RPOSITION,
        SPOSITION,
        TEAMCHANGE,
        CHARCHANGE
    };

public:
    static std::pair<int, int> command(msgpack::object &deserialized);
};

#endif//CHATTING_SERVER_MESSAGE_H

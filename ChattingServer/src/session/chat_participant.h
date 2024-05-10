#include "../../include/msgpack.hpp"

class ChatParticipant
{
public:
    virtual ~ChatParticipant() {}
    virtual void deliver(const std::string& msg) = 0;
};

typedef boost::shared_ptr<ChatParticipant> chat_participant_ptr;

package ssafy.authserv.domain.friendship.dto;

import java.util.UUID;

public record FriendResponseEvent(
        UUID receiverId,
        String sender,
        String receiver,
        String status
) {
}

package ssafy.authserv.domain.friendship.dto;

import ssafy.authserv.domain.friendship.entity.enums.FriendshipStatus;

import java.io.Serial;
import java.util.UUID;

public record FriendResponse(
        UUID messageReceiverId,
        String sender,
        String receiver,
        String status
) {
}

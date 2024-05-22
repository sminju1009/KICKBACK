package ssafy.authserv.domain.friendship.dto;

import ssafy.authserv.domain.friendship.entity.enums.FriendshipStatus;

import java.util.UUID;

public record FriendResponse(
        String sender,
        String receiver,
        String status
) {
}

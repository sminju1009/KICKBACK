package ssafy.authserv.domain.friendship.dto;

import java.util.UUID;

public record FriendRequestEvent(
        UUID requesterId,
        UUID receiverId,
        String requesterNickname,
        String receiverNickname
) {
}

package ssafy.authserv.domain.friendship.dto;

import ssafy.authserv.domain.friendship.entity.enums.FriendshipStatus;
import ssafy.authserv.domain.member.entity.enums.MemberRole;

public record FriendReply(
        String receiver,
        String requester,
        String status
) {
    public static FriendshipStatus getStatus(String status){
        return FriendshipStatus.valueOf(status.toUpperCase());
    }
}

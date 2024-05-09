package ssafy.authserv.domain.friendship.entity.enums;


public enum FriendshipStatus {
    REQUESTED, ACCEPTED, REJECTED, BLOCKED, PENDING;

    public static FriendshipStatus fromName(String status) {
        return FriendshipStatus.valueOf(status.toUpperCase());
    }
}


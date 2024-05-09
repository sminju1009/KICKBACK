package ssafy.authserv.domain.friendship.service;

import ssafy.authserv.domain.friendship.dto.FriendResponse;
import ssafy.authserv.domain.friendship.entity.Friendship;

import java.util.UUID;

public interface FriendshipService {

    void sendFriendRequest(UUID senderId, String sederNickName, String receiverNickname);

    void updateFriendshipStatus(FriendResponse response);
}
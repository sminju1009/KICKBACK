package ssafy.authserv.domain.friendship.service;

import ssafy.authserv.domain.friendship.dto.FriendReply;
import ssafy.authserv.domain.friendship.dto.FriendResponse;

import java.util.UUID;


public interface FriendshipService {

    FriendResponse sendFriendRequest(UUID requesterId, String receiverName);

    FriendResponse updateFriendshipStatus(FriendReply accept);
}

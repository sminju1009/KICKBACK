package ssafy.authserv.domain.friendship.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.stereotype.Service;
import ssafy.authserv.domain.friendship.dto.FriendRequest;
import ssafy.authserv.domain.friendship.dto.FriendRequestEvent;
import ssafy.authserv.domain.friendship.dto.FriendResponse;

import java.util.UUID;

@Service
@Slf4j
@RequiredArgsConstructor
public class NotificationService {
    private final SimpMessagingTemplate messagingTemplate;

    // messageReceiverId는 알림 응답 받는 대상
    @KafkaListener(topics = "friendshipRequest", groupId = "friendship-service")
    public void processFriendRequest(FriendResponse response) {
        log.info("Processing friend request from {} to {}", response.sender(), response.receiver());

        messagingTemplate.convertAndSendToUser(response.messageReceiverId().toString(), "/queue/friendship",
                response);
        log.info("보냄: {}", response.messageReceiverId());
    }

    @KafkaListener(topics = "friendshipUpdate", groupId = "friendship-service")
    public void processFriendRequestReply(FriendResponse response) {
        log.info("Processing friendship update. Requester: {}, Receiver: {}, result: {}", response.receiver(), response.sender(), response.status());

        messagingTemplate.convertAndSendToUser(response.messageReceiverId().toString(), "/queue/friendship", response );
    }
}

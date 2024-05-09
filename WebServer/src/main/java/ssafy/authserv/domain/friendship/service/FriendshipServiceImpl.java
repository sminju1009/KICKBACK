package ssafy.authserv.domain.friendship.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.kafka.core.KafkaTemplate;
import org.springframework.stereotype.Service;
import ssafy.authserv.domain.friendship.dto.FriendResponse;
import ssafy.authserv.domain.friendship.entity.Friendship;
import ssafy.authserv.domain.friendship.entity.enums.FriendshipStatus;
import ssafy.authserv.domain.friendship.repository.FriendshipRepository;
import ssafy.authserv.domain.member.entity.Member;
import ssafy.authserv.domain.member.repository.MemberRepository;

import java.util.UUID;

@Service
@RequiredArgsConstructor
@Slf4j
public class FriendshipServiceImpl implements FriendshipService {

    private final MemberRepository memberRepository;
    private final FriendshipRepository friendshipRepository;

    private final KafkaTemplate<String, Friendship> kafkaTemplate;

    @Override
    public void sendFriendRequest(UUID senderId, String sederNickName, String receiverNickname) {
        Member sender = memberRepository.findById(senderId)
                .orElseThrow(() -> new RuntimeException("User not found with ID: " + senderId));
        Member receiver = memberRepository.findByNickname(receiverNickname)
                .orElseThrow(() -> new RuntimeException("User not found with the name: " + receiverNickname));

        Friendship friendship = Friendship.builder()
                .member(sender)
                .friend(receiver)
                .status(FriendshipStatus.PENDING)
                .build();
        friendshipRepository.save(friendship);
        kafkaTemplate.send("user-" + receiver.getId(), friendship);
    }

    @Override
    public void updateFriendshipStatus(FriendResponse response) {
    }
}

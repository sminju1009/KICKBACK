//package org.example.businessserver.friendship;
//
//import lombok.RequiredArgsConstructor;
//import org.example.businessserver.friendship.enums.Status;
//import org.springframework.kafka.annotation.KafkaListener;
//import org.springframework.kafka.core.KafkaTemplate;
//import org.springframework.stereotype.Service;
//
//@Service
//@RequiredArgsConstructor
//public class FriendService {
//    private KafkaTemplate<String, String> kafkaTemplate;
//    private final FriendshipRepository friendshipRepository;
//
//    public void handleFriendRequest(String requesterId, String responderId) {
//        String message = String.format("Request from %s to %s", requesterId, responderId);
//        kafkaTemplate.send("friend-requests", message);
//        friendshipRepository.save(
//                FriendshipDoc.builder()
//                        .requesterId(requesterId)
//                        .responderId(responderId)
//                        .status(Status.PENDING)
//                        .build()
//        );
//    }
//
//    public void acceptFriendRequest(String id){
//        FriendshipDoc friendship = friendshipRepository.findById(id).orElseThrow();
//        friendship.setStatus(Status.ACCEPTED);
//        friendshipRepository.save(friendship);
//        kafkaTemplate.send("friend-responses", "Accepted request from " + friendship.getRequesterId() + " to " + friendship.getResponderId());
//    }
////
//    public void rejectFriednRequest(String id){
//        FriendshipDoc friendship = friendshipRepository.findById(id).orElseThrow();
//        friendship.setStatus(Status.REJECTED);
//        friendshipRepository.save(friendship);
//        kafkaTemplate.send("friend-responses", "Rejected request from " + friendship.getRequesterId() + " to " + friendship.getResponderId());
//    }
//
//    @KafkaListener(topics = "friend-responses", groupId = "${spring.kafka.consumer.group-id}")
//    public void processFriendResponses(String message) {
//        System.out.println("Received response: " + message);
//    }
//}

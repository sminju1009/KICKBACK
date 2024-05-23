package ssafy.authserv.global.config;

import org.apache.kafka.common.serialization.StringDeserializer;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.kafka.config.ConcurrentKafkaListenerContainerFactory;
import org.springframework.kafka.core.ConsumerFactory;
import org.springframework.kafka.core.DefaultKafkaConsumerFactory;
import org.springframework.kafka.support.serializer.JsonDeserializer;
import ssafy.authserv.domain.friendship.dto.FriendResponse;

import java.util.HashMap;
import java.util.Map;

@Configuration
public class KafkaConsumerConfig {

    @Value("${spring.kafka.consumer.bootstrap-servers}")
    private String bootstrapServers;

    @Value("${spring.kafka.consumer.group-id}")
    private String groupId;

    @Value("${spring.kafka.consumer.auto-offset-reset}")
    private String autoOffsetRest;

    @Bean
    public ConsumerFactory<String, FriendResponse> consumerFactory() {
        Map<String, Object> configProps = new HashMap<>();
        configProps.put("bootstrap.servers", bootstrapServers);
        configProps.put("group.id", groupId);
        configProps.put("key.deserializer", StringDeserializer.class);
//        configProps.put("value.deserializer", JsonDeserializer.class);
        configProps.put("auto.offset.reset", autoOffsetRest);

        // 이거 문법 확인하기
        JsonDeserializer<FriendResponse> deserializer = new JsonDeserializer<>(FriendResponse.class);
        deserializer.addTrustedPackages("*");

        configProps.put("value.deserializer", deserializer);

        return new DefaultKafkaConsumerFactory<>(configProps, new StringDeserializer(), deserializer);
    }

    @Bean
    public ConcurrentKafkaListenerContainerFactory<String, FriendResponse> kafkaListenerContainerFactory() {
        ConcurrentKafkaListenerContainerFactory<String, FriendResponse> factory = new ConcurrentKafkaListenerContainerFactory<>();
        factory.setConsumerFactory(consumerFactory());

        return factory;
    }
}

package ssafy.authserv.global.config;

import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.common.serialization.StringDeserializer;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.kafka.annotation.EnableKafka;
import org.springframework.kafka.config.ConcurrentKafkaListenerContainerFactory;
import org.springframework.kafka.core.ConsumerFactory;
import org.springframework.kafka.core.DefaultKafkaConsumerFactory;
import org.springframework.kafka.support.serializer.JsonDeserializer;
import ssafy.authserv.domain.friendship.entity.Friendship;

import java.util.HashMap;
import java.util.Map;

@Configuration
@EnableKafka
@Slf4j
//@RequiredArgsConstructor
public class KafkaConsumerConfig {

    @Value("spring.kafka.consumer.bootstrap-servers")
    private  String bootstrapServers;

    @Value("${spring.kafka.consumer.group-id}")
    private String groupId;

    @Value("${spring.kafka.consumer.auto-offset-reset}")
    private String autoOffsetReset;

    @Bean
    public ConsumerFactory<String, Friendship> consumerFactory() {
        Map<String, Object> configProps = new HashMap<>();
        configProps.put("bootstrap.servers", bootstrapServers);
        configProps.put("group.id", groupId);
        configProps.put("key.deserializer", StringDeserializer.class);
        configProps.put("value.deserializer", JsonDeserializer.class);
        configProps.put("auto.offset.reset", autoOffsetReset);

        return new DefaultKafkaConsumerFactory<>(configProps);
    }

    @Bean
    public ConcurrentKafkaListenerContainerFactory<String, Friendship> kafkaListenerContainerFactory() {
        ConcurrentKafkaListenerContainerFactory<String, Friendship> factory
                = new ConcurrentKafkaListenerContainerFactory<>();
        factory.setConsumerFactory(consumerFactory());

        return factory;
    }

//    @KafkaListener(topics = "friendshipUpdates", groupId = "friendship-service")
//    public void consume(String message){
//        log.info("Received message: {}", message);
//    }
}

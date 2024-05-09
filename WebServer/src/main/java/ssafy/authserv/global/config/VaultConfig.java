package ssafy.authserv.global.config;

import lombok.RequiredArgsConstructor;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.vault.authentication.TokenAuthentication;
import org.springframework.vault.client.VaultEndpoint;
import org.springframework.vault.core.VaultTemplate;
import ssafy.authserv.global.vault.VaultProperties;

import java.net.URI;

@EnableConfigurationProperties(VaultProperties.class)
@RequiredArgsConstructor
@Configuration
public class VaultConfig {
    private final VaultProperties vaultProperties;

    @Bean
    public VaultTemplate vaultTemplate() throws IllegalArgumentException {
        return new VaultTemplate(
                VaultEndpoint.from(URI.create(vaultProperties.uri())),
                new TokenAuthentication(vaultProperties.token())
        );
    }
}

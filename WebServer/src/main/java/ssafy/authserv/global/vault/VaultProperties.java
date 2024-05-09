package ssafy.authserv.global.vault;

import org.springframework.boot.context.properties.ConfigurationProperties;

@ConfigurationProperties(prefix = "vault")
public record VaultProperties(
        String uri,
        String token,
        String keyPath
) {
}

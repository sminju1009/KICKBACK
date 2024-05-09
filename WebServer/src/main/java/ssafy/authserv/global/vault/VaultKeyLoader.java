package ssafy.authserv.global.vault;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Component;
import org.springframework.vault.core.VaultTemplate;
import org.springframework.vault.support.VaultResponse;

import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;
import java.util.Base64;
import java.util.Map;

@Component
@RequiredArgsConstructor
@Slf4j
public class VaultKeyLoader {
    private final VaultTemplate vaultTemplate;

    public SecretKey loadSecretKey(String keyPath) {
        VaultResponse response = vaultTemplate.read(keyPath);
        if (response == null || response.getData() == null) {
            throw new IllegalArgumentException("Key not found in Vault: " + keyPath);
        }

        Object responseDataObject = response.getData().get("data");
        if (!(responseDataObject instanceof Map)) {
            throw new IllegalArgumentException("No data map available at path: " + keyPath);
        }

        Map<String, Object> responseData = (Map<String, Object>) responseDataObject;

        String encodedKey = (String) responseData.get("key");
        if (encodedKey == null) {
            throw new IllegalArgumentException("Encoded key is null at path: " + keyPath);
        }
        byte[] keyBytes = Base64.getDecoder().decode(encodedKey);

        if (keyBytes.length != 16 && keyBytes.length != 24 && keyBytes.length != 32) {
            throw new IllegalArgumentException("Invalid AES key length: " + keyBytes.length + " bytes");
        }

        return new SecretKeySpec(keyBytes, "AES");
    }
}

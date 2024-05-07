package ssafy.authserv.global.jwt.repository;

import lombok.RequiredArgsConstructor;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Repository;
import ssafy.authserv.global.jwt.JwtProps;
import ssafy.authserv.global.vault.VaultKeyLoader;
import ssafy.authserv.global.vault.VaultProperties;

import javax.crypto.*;
import java.nio.charset.StandardCharsets;
import java.security.GeneralSecurityException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;
import java.util.Optional;

@Repository
@RequiredArgsConstructor
public class RefreshTokenRepository {

    private final RedisTemplate<String, String> redisTemplate;
    private static final String KEY_PREFIX = "refreshToken::";
    private final JwtProps jwtProps;

    private static final String AES_TRANSFORMATION = "AES";
    private final VaultProperties vaultProperties;
    private final VaultKeyLoader vaultKeyLoader;

    public void save(String email, String token) throws GeneralSecurityException {
        String key = KEY_PREFIX + email;
        byte[] encrypedToken = encryptToken(token);
        redisTemplate.opsForValue().set(key, Base64.getEncoder().encodeToString(encrypedToken), jwtProps.refreshExpiration());
    }

    public Optional<String> find(String email) throws GeneralSecurityException{
        String key = KEY_PREFIX + email;
        String encodedToken = redisTemplate.opsForValue().get(key);
        if(encodedToken == null){
            return Optional.empty();
        }
        String decryptedToken = decryptToken(encodedToken);
        return Optional.of(decryptedToken);
    }

    public void delete(String email) throws NoSuchAlgorithmException {
        redisTemplate.delete(KEY_PREFIX + email);
    }

    private byte[] encryptToken(String token) throws NoSuchPaddingException, NoSuchAlgorithmException, IllegalBlockSizeException, BadPaddingException, InvalidKeyException {
        Cipher cipher = Cipher.getInstance(AES_TRANSFORMATION);
        cipher.init(Cipher.ENCRYPT_MODE, getSecretKey());
        return cipher.doFinal(token.getBytes(StandardCharsets.UTF_8));
    }

    private SecretKey getSecretKey() {
        return vaultKeyLoader.loadSecretKey(vaultProperties.keyPath());
    }

    private String decryptToken(String encodedToken) throws GeneralSecurityException {
        Cipher cipher = Cipher.getInstance(AES_TRANSFORMATION);
        cipher.init(Cipher.DECRYPT_MODE, getSecretKey());
        byte[] decryptedTokenBites = cipher.doFinal(Base64.getDecoder().decode(encodedToken));
        return new String(decryptedTokenBites);
    }
}

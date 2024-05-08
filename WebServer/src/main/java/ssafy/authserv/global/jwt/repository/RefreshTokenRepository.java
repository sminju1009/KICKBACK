package ssafy.authserv.global.jwt.repository;

import lombok.RequiredArgsConstructor;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Repository;
import ssafy.authserv.global.jwt.JwtProps;
import ssafy.authserv.global.vault.VaultKeyLoader;
import ssafy.authserv.global.vault.VaultProperties;

import javax.crypto.*;
import javax.crypto.spec.GCMParameterSpec;
import java.nio.charset.StandardCharsets;
import java.security.*;
import java.util.Arrays;
import java.util.Base64;
import java.util.Optional;

@Repository
@RequiredArgsConstructor
public class RefreshTokenRepository {

    private final RedisTemplate<String, String> redisTemplate;
    private static final String KEY_PREFIX = "refreshToken::";
    private final JwtProps jwtProps;

    private static final String AES_TRANSFORMATION = "AES/GCM/NoPadding";
    private final VaultProperties vaultProperties;
    private final VaultKeyLoader vaultKeyLoader;

    public void save(String email, String token) throws GeneralSecurityException {
        String key = KEY_PREFIX + email;
        byte[] encryptedToken = encryptToken(token);
        redisTemplate.opsForValue().set(key, Base64.getEncoder().encodeToString(encryptedToken), jwtProps.refreshExpiration());
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

    public void delete(String email){
        redisTemplate.delete(KEY_PREFIX + email);
    }

    private byte[] encryptToken(String token) throws NoSuchPaddingException, NoSuchAlgorithmException, IllegalBlockSizeException, BadPaddingException, InvalidKeyException, InvalidAlgorithmParameterException {

        Cipher cipher = Cipher.getInstance(AES_TRANSFORMATION);
        byte[] iv = new byte[12]; // GCM recommended IV length is 12 bytes
        new SecureRandom().nextBytes(iv);
        GCMParameterSpec spec = new GCMParameterSpec(128, iv); // 128-bit auth tag length
        cipher.init(Cipher.ENCRYPT_MODE, getSecretKey(), spec);
        byte[] encrypted = cipher.doFinal(token.getBytes(StandardCharsets.UTF_8));
        byte[] encryptedIVAndText = new byte[iv.length + encrypted.length];
        System.arraycopy(iv, 0, encryptedIVAndText, 0, iv.length);
        System.arraycopy(encrypted, 0, encryptedIVAndText, iv.length, encrypted.length);
        return encryptedIVAndText;
    }

    private SecretKey getSecretKey() {
        return vaultKeyLoader.loadSecretKey(vaultProperties.keyPath());
    }

    private String decryptToken(String encodedToken) throws GeneralSecurityException {
        byte[] encryptedIVAndText = Base64.getDecoder().decode(encodedToken);
        Cipher cipher = Cipher.getInstance(AES_TRANSFORMATION);
        byte[] iv = Arrays.copyOfRange(encryptedIVAndText, 0, 12);
        GCMParameterSpec spec = new GCMParameterSpec(128, iv);
        cipher.init(Cipher.DECRYPT_MODE, getSecretKey(), spec);
        byte[] decrypted = cipher.doFinal(encryptedIVAndText, 12, encryptedIVAndText.length - 12);
        return new String(decrypted, StandardCharsets.UTF_8);
    }
}

package ssafy.authserv.global.jwt.service;

import ssafy.authserv.domain.member.dto.LoginResponse;
import ssafy.authserv.domain.member.entity.Member;

import java.security.GeneralSecurityException;

public interface JwtTokenService {
    LoginResponse issueAndSaveTokens(Member member);

    String reissueAccessToken(String email) throws GeneralSecurityException;
}

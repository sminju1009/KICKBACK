package ssafy.authserv.domain.friendship.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import ssafy.authserv.domain.friendship.entity.Friendship;
import ssafy.authserv.domain.member.entity.Member;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Repository
public interface FriendshipRepository extends JpaRepository<Friendship, Long> {
    /**
     *  실제 데이터베이스 조회에는 Member 객체의 ID가 사용됩니다.
     *  즉, Member 객체 내의 id 필드가 자동으로 추출되어 해당 ID를 기준으로 Friendship 객체를 조회합니다.
     */
    Optional<Friendship> findByMember(Member member);

}

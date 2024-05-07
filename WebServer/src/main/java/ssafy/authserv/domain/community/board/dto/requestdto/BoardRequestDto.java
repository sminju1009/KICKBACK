package ssafy.authserv.domain.community.board.dto.requestdto;

import lombok.Getter;

import java.time.LocalDateTime;

@Getter
public class BoardRequestDto {
    private String title;
    private String content;
    private String category;

    public BoardCategory getCategory() {
        return BoardCategory.fromName(this.category);
    }
    private LocalDateTime updatedAt;
    private LocalDateTime createdAt;
}
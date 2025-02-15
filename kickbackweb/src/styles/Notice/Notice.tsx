import styled from 'styled-components';

const NoticeBox = styled.div`
  display: flex;
  flex-direction: column;
  padding: 20px;
  margin: 20px;
  margin-bottom: 80px;
  font-family: 'LINESeedKR-Bd' !important;

//   border: 1px solid #ccc;
//   background-color: #f9f9f9;
`;

const NoticeItem = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between; /* Ensures separation between left and right contents */
  padding: 10px;
  border-bottom: 1px solid #eee;
  cursor: pointer;
  font-family: 'LINESeedKR-Bd' !important;

  transition: background-color 0.3s; // 배경색 변경에 애니메이션 효과를 줍니다.

  &:hover {
    background-color: #f0f0f0; // 호버 시 배경색을 변경합니다.
  }

  .notice-content {
    display: flex;
    align-items: center;
    flex-grow: 1;
    font-family: 'LINESeedKR-Bd' !important;

  }

  .notice-date {
    display: flex;
    flex-direction: column;
    align-items: flex-end; /* Aligns text to the right */
    text-align: right; /* Ensures text is right-aligned */
    font-family: 'LINESeedKR-Bd' !important;
  }

  img {
    width: 50px;
    height: 50px;
    border-radius: 25px;
    object-fit: cover;
    margin-right: 15px; /* Space between image and text */
  }
`;

const CategorySelector = styled.div`
  display: flex;
  justify-content: flex-start; /* Align items to the left */
  border-bottom: 1px solid #ccc; // Line at the bottom of the tabs for unselected state
  margin-bottom: 20px;
  font-family: 'LINESeedKR-Bd' !important;

  .item {
    flex-grow: 1;
    padding: 10px 0;
    text-align: center;
    cursor: pointer;
    background-color: #f9f9f9;
    transition: background-color 0.3s, border-color 0.3s;
    border-top: 3px solid transparent; // Default state
    font-family: 'LINESeedKR-Bd' !important;

    &:hover {
    //   background-color: #e0e0e0;
      background-color: #FFFFFF;
    }

    &.choice1 {
      border-top-color: blue;
      background-color: #FFFFFF;
    }

    &.choice2 {
      border-top-color: #D7DB49;
      background-color: #FFFFFF;
    }

    &.choice-all {
      border-top-color: gray; // Color for 'ALL' option
      background-color: #FFFFFF;
    }
  }
`;

const StyledNoticeHeader = styled.div`
    background-color: #e0e0e0;
    width: 95%;
    margin: 20px 20px 5px 20px;
    height: auto;
    display: flex;
    flex-direction: row;
    padding: 0 10px;

    display: flex;
    align-items: center;
    gap: 20px;
    padding-bottom: 5px;
    border-bottom: 1px solid #ccc;
    font-family: 'LINESeedKR-Bd' !important;

    img {
        width: 25px;
        height: 25px;
        border-radius: 25px;
        object-fit: cover;
        margin-left: 15px; /* Space between image and text */
      }

    h1 {
        flex-grow: 1;
        font-family: 'LINESeedKR-Bd' !important;

    }

    span {
        font-size: 0.5em;
        color: #666;
        margin-right: 10px;
        font-family: 'LINESeedKR-Bd' !important;

    }
`;

const NoticeDetailBox = styled.div`
    width: 90%;
    margin: 40px auto;
    height: auto;

    display: flex;
    flex-direction: column;
    margin-bottom: 80px;
    justify-content: center;
    align-items: center;
    font-family: 'LINESeedKR-Bd' !important;
    
`

const NoticeContentBox = styled.pre`
    width: 95%;
    margin: 100px auto;
    height: auto;
    display: flex;
    flex-direction: row;
    // border-bottom: 1px solid #eee;
    font-family: 'LINESeedKR-Bd' !important;

    .content {
        flex: 1;
        display: flex;
        flex-direction: column;
        border: 1px solid lightgray;
        border-radius: 5px;
    }
`

// const NavigationButtons = styled.button`
//     display: flex;
//     justify-content: space-between;
//     margin-top: 20px;
// `;
const NavigationButtons = styled.button`
    background-color: #f0f0f0;
    border: 1px solid #ccc;
    padding: 10px 20px;
    cursor: pointer;
    margin-right: 10px; // 버튼 사이 간격
    font-family: 'LINESeedKR-Bd' !important;

    &:last-child {
        margin-right: 0; // 마지막 버튼의 오른쪽 여백 제거
    }

    &:hover {
        background-color: #e0e0e0;
    }
    border-radius: 1rem;
`;

const ButtonsContainer = styled.div`
    width: 95%;
    display: flex;
    justify-content: flex-start; // 버튼을 좌측 정렬
    // position: absolute; // 상위 요소 기준으로 절대 위치
    // bottom: 10px; // 하단에서 10px 떨어진 위치
    // left: 10px; // 좌측에서 10px 떨어진 위치
`;

const BottomBox = styled.div`
  width: 95%;
  margin: 0 auto;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  border-top: 1px solid #ccc;
  padding-top: 5px;
  font-family: 'LINESeedKR-Bd' !important;
  
  .list {
    width: 60px;
    height: auto;
    text-align: center;
    border: 1px solid #ccc;
    border-radius: 1rem;
    padding: 10px;
    background-color: #f0f0f0;
    font-size: 15px;
    cursor: pointer;
    font-family: 'LINESeedKR-Bd' !important;

    &:hover {
      background-color: #dcdada;
    }
  }
`



export {NoticeBox, NoticeItem, CategorySelector, StyledNoticeHeader, NavigationButtons, NoticeDetailBox, NoticeContentBox, ButtonsContainer, BottomBox};
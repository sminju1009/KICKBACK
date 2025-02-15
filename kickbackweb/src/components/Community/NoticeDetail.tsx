// import React, { useEffect, useState } from "react";
// import axios from "axios";
// import { useParams, useNavigate } from "react-router-dom";
// import useAuthStore from "../../stores/AuthStore";
// import useNoticeStore from "../../stores/NoticStore";
// import { useShallow } from "zustand/react/shallow";
// import NoticeIcon from "../../assets/noticeIcon.png";
// import UpdateIcon from "../../assets/updateIcon.png";
// import {
//   NavigationButtons,
//   StyledNoticeHeader,
//   NoticeDetailBox,
//   NoticeContentBox,
//   ButtonsContainer,
//   BottomBox,
// } from "../../styles/Notice/Notice";
// import * as s from "../../styles/Community/Board";
// import noti from "../../assets/CebuTrack.png";

// interface NoticeDetailData {
//   id: number;
//   title: string;
//   content: string;
//   category: "NOTICE" | "UPDATE" | "ALL";
//   nickname: string;
//   createdAt: string;
//   updatedAt: string;
// }

// const NoticeDetail = () => {
//   const { noticeId } = useParams<{ noticeId: string }>();
//   const { PATH } = useAuthStore(useShallow((state) => ({ PATH: state.PATH })));
//   const [noticeDetail, setNoticeDetail] = useState<NoticeDetailData | null>(
//     null
//   );

//   const { minId, maxId, setMinId, setMaxId } = useNoticeStore(
//     useShallow((state) => ({
//       minId: state.minId, // Corrected typo here from 'setMindId' to 'setMinId'
//       maxId: state.maxId,
//       setMinId: state.setMinId,
//       setMaxId: state.setMaxId,
//     }))
//   );

//   const navigate = useNavigate();

//   useEffect(() => {
//     // minId와 maxId를 가져오는 로직
//     // axios.get(`http://localhost:8080/api/v1/notice/all`)
//     axios
//       .get(`${PATH}/api/v1/notice/all`)
//       .then((response) => {
//         const notices = response.data;
//         if (notices.length > 0) {
//           setMinId(notices[0].id);
//           setMaxId(notices[notices.length - 1].id);
//         }
//       })
//       .catch((error) => {
//         console.error("Error fetching notices:", error);
//       });
//   }, []);

//   useEffect(() => {
//     if (noticeId !== undefined && minId !== undefined && maxId !== undefined) {
//       const noticeIdInteger = parseInt(noticeId, 10);
//       if (!isNaN(noticeIdInteger)) {
//         // Check if `noticeIdInteger` is a valid number
//         console.log(noticeIdInteger);
//         // axios.get(`http://localhost:8080/api/v1/notice/${noticeIdInteger}`) // Use the valid integer for API request
//         axios
//           .get(`${PATH}/api/v1/notice/${noticeIdInteger}`) // Use the valid integer for API request
//           .then((response) => {
//             setNoticeDetail(response.data);
//           })
//           .catch((error) => {
//             console.error("Error fetching notice details:", error);
//           });
//       }
//     }
//   }, [PATH, noticeId, minId, maxId]);

//   const getIconByCategory = (category: "NOTICE" | "UPDATE" | "ALL"): string => {
//     switch (category) {
//       case "NOTICE":
//         return NoticeIcon;
//       case "UPDATE":
//         return UpdateIcon;
//       default:
//         return NoticeIcon; // Default 로고
//     }
//   };

//   if (!noticeDetail) {
//     return <div>Loading...</div>;
//   }

//   return (
//     <>
//       <s.IntroBox>
//         <s.ImgBox>
//           <img src={noti} alt="커뮤" />
//           <div className="text">
//             {noticeDetail.category === "NOTICE" ? "공지사항" : "업데이트"}
//           </div>
//         </s.ImgBox>
//       </s.IntroBox>
//       <NoticeDetailBox>
//         <StyledNoticeHeader>
//           <img
//             src={getIconByCategory(noticeDetail.category)}
//             alt="Category Icon"
//           />
//           <h1>{noticeDetail.title}</h1>
//           <span>
//             작성일: {new Date(noticeDetail.createdAt).toLocaleDateString()}
//           </span>
//         </StyledNoticeHeader>
//         <NoticeContentBox>{noticeDetail.content}</NoticeContentBox>
//         <BottomBox>
//           <ButtonsContainer>
//             {noticeDetail.id > minId && (
//               <NavigationButtons
//                 onClick={() => navigate(`/notice/${noticeDetail.id - 1}`)}
//               >
//                 Prev
//               </NavigationButtons>
//             )}
//             {noticeDetail.id < maxId && (
//               <NavigationButtons
//                 onClick={() => navigate(`/notice/${noticeDetail.id + 1}`)}
//               >
//                 Next
//               </NavigationButtons>
//             )}
//           </ButtonsContainer>
//           <div className="list" onClick={() => navigate("/notice")}>
//             목록
//           </div>
//         </BottomBox>
//       </NoticeDetailBox>
//     </>
//   );
// };

// export default NoticeDetail;

import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";
import useAuthStore from "../../stores/AuthStore";
import NoticeIcon from "../../assets/noticeIcon.png";
import UpdateIcon from "../../assets/updateIcon.png";
import {
  NavigationButtons,
  StyledNoticeHeader,
  NoticeDetailBox,
  NoticeContentBox,
  ButtonsContainer,
  BottomBox,
} from "../../styles/Notice/Notice";
import * as s from "../../styles/Community/Board";
import noti from "../../assets/CebuTrack.png";

interface NoticeDetailData {
  id: number;
  title: string;
  content: string;
  category: "NOTICE" | "UPDATE" | "ALL";
  nickname: string;
  createdAt: string;
  updatedAt: string;
  prevNoticeId: number | null;
  nextNoticeId: number | null;
}

const NoticeDetail = () => {
  const { noticeId } = useParams<{ noticeId: string }>();
  const { PATH } = useAuthStore((state) => ({ PATH: state.PATH }));
  const [noticeDetail, setNoticeDetail] = useState<NoticeDetailData | null>(
    null
  );
  const navigate = useNavigate();

  useEffect(() => {
    if (noticeId !== undefined) {
      const noticeIdInteger = parseInt(noticeId, 10);
      if (!isNaN(noticeIdInteger)) {
        axios
          .get(`${PATH}/api/v1/notice/${noticeIdInteger}`)
          .then((response) => {
            setNoticeDetail(response.data);
          })
          .catch((error) => {
            console.error("Error fetching notice details:", error);
          });
      }
    }
  }, [PATH, noticeId]);

  const getIconByCategory = (category: "NOTICE" | "UPDATE" | "ALL"): string => {
    switch (category) {
      case "NOTICE":
        return NoticeIcon;
      case "UPDATE":
        return UpdateIcon;
      default:
        return NoticeIcon;
    }
  };

  if (!noticeDetail) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <s.IntroBox>
        <s.ImgBox>
          <img src={noti} alt="커뮤" />
          <div className="text">
            {noticeDetail.category === "NOTICE" ? "공지사항" : "업데이트"}
          </div>
        </s.ImgBox>
      </s.IntroBox>
      <NoticeDetailBox>
        <StyledNoticeHeader>
          <img
            src={getIconByCategory(noticeDetail.category)}
            alt="Category Icon"
          />
          <h1>{noticeDetail.title}</h1>
          <span>
            작성일: {new Date(noticeDetail.createdAt).toLocaleDateString()}
          </span>
        </StyledNoticeHeader>
        <NoticeContentBox>{noticeDetail.content}</NoticeContentBox>
        <BottomBox>
          <ButtonsContainer>
            {noticeDetail.nextNoticeId && (
              <NavigationButtons
                onClick={() => navigate(`/notice/${noticeDetail.nextNoticeId}`)}
              >
                이전
              </NavigationButtons>
            )}
            {noticeDetail.prevNoticeId && (
              <NavigationButtons
                onClick={() => navigate(`/notice/${noticeDetail.prevNoticeId}`)}
              >
                다음
              </NavigationButtons>
            )}
          </ButtonsContainer>
          <div className="list" onClick={() => navigate("/notice")}>
            목록
          </div>
        </BottomBox>
      </NoticeDetailBox>
    </>
  );
};

export default NoticeDetail;

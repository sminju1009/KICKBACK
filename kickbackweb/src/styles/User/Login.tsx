import styled from "styled-components";

const LogoBox = styled.div`
  width: 100%;
  height: auto;
  text-align: center;
  position: relative;
  margin-bottom: 50px;
  img {
    width: 40%;
  }
`

const LoginBackBox = styled.div`
  width: 40%;
  height: auto;
  margin: 0 auto;
  background-color: #393939;
  border: 0;
  border-radius: 5px;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;

  .item {
    width: 90%;
    margin: 0 auto;
    padding: 20px;
    margin-bottom: 15px;
    color: #bbbbbb;
    
    display: flex; 
    flex-direction: row;
    align-items: center;
    position: relative;

    .content {
      flex: 1;
      position: relative;
    }

    .content:nth-child(1) {
      flex: 15%;
    }

    .content:nth-child(2) {
      flex: 85%;
      width: 100%;
      height: 50px;
      background-color: #e1e0e0;
      color: black;
      border: 0;
      border-radius: 3px;
      font-size: 20px;
    }

    .btn {
      position: relative;
      width: 100%;
      margin: 0 auto;
      padding: 20px 0;
      text-align: center;
      background-color: #0278f6;
      color: white;
      font-size: 27px;
      border-radius: 5px;
      cursor: pointer;
    }

    .abs {
      position: absolute;
      color: #757474;
      right: 5%;
      font-size: 30px;
      cursor: pointer;
    }
  }
`

const SignUpBackBox = styled.div`
  width: 50%;
  margin: 0 auto;
  background-color: #393939;
  border: 0;
  border-radius: 10px;
  margin-top: 30px;

  display: flex;
  flex-direction: column;
  justify-content: center;

  .item {
    padding: 20px;
    margin-bottom: 30px;
    color: #bbbbbb;

    display: flex; 
    flex-direction: row;
    align-items: center;
    .content {
      flex: 1;
    }

    .content:nth-child(1) {
      flex: 10%;
    }

    .content:nth-child(2) {
      flex: 90%;
      height: 50px;
      background-color: white;
      border: 0;
      border-radius: 10px;
    }

    .btn {
      width: 98%;
      margin: 0 auto;
      padding: 10px 0;
      text-align: center;
      background-color: #0278f6;
      color: white;
      font-size: 27px;
    }
  }
`

export {LogoBox, LoginBackBox, SignUpBackBox};
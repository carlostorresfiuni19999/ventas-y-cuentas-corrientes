const login = (username, password) => {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/x-www-form-urlencoded");

    const urlencoded = new URLSearchParams();
    urlencoded.append("userName", `${username}`);
    urlencoded.append("password", `${password}`);
    urlencoded.append("grant_type", "password");
    myHeaders.append("Cookie", ".AspNet.Cookies=wsvwvzdAckOhCFLlkdpcJNwu1V0ZF6kS_9rzjJ05F45BOI2UNd1c40ingMY4PAqqLn3tqzXThRJLTReaot442Sb7Nxu2mc5BjJ2yBFMo0Ws3oHL4zt71MFj0zwFS0UPtxe1ipfUi_w4yJOtC_rI1BZ_tin-3vOxNlNrKYodroVl76rpi6xBXYVr9RAlvayWU6NyUORfm7VkCLPKrc0lRkehCSNKQZVwgk_d8aY-N07QvlY1f26geazDBFM_YiPTu4szlDi3YDfjFmM4twFxp3Nu0OTcWHhwTydDZ4Ua2Lu5AyLbYghsF3gkZm210G6RcgElWPATkYcG5G7vftjF1KwWYNIGrwbrvZUIs0fw5EDqqMzrRApe4uEKzyDooKvzjG_Z8q8KaqLyEXBfoRcd1sHkBb8w3ihpuj6OkF7WTI0nsWKktlkB6i6tEE8168U3uQxWRuKHL7UvV_6AliUTmcVKAz_g5VMBaFlI9hCngIb7NZ0_zs0ejE-M3p-AWPVn1aw3B_eu1K_4TgR7y9LXOcw; ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");

    console.log(urlencoded);
    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: urlencoded,
        redirect: 'follow'
    };

     return fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/Token", requestOptions)
}

export default login;

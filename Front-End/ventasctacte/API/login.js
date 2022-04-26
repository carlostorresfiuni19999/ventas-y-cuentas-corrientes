const login = (username, password) => {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/x-www-form-urlencoded");

    const urlencoded = new URLSearchParams();
    urlencoded.append("userName", `${username}`);
    urlencoded.append("password", `${password}`);
    urlencoded.append("grant_type", "password");

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: urlencoded,
        redirect: 'follow'
    };

     return fetch("https://cuentasctacte-web-api20220425205158.azurewebsites.net/Token", requestOptions)
}

export default login;

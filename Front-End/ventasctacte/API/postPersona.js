const PostPersona = (persona, token) => {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${token}`);
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify({
        "Nombre": `${persona.Nombre}`,
        "Apellido": `${persona.Apellido}`,
        "Doc": `${persona.Doc}`,
        "DocumentoTipo": `${persona.DocumentoTipo}`,
        "LineaDeCredito": persona.LineaDeCredito,
        "Roles": persona.Roles,
        "Telefono": `${persona.Telefono}`,
        "UserName": `${persona.UserName}`,
        "Password": `${persona.Password}`,
        "ConfirmPassword":`${persona.ConfirmPassword}`
      }
    );

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

  fetch("http://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Account/Register", requestOptions);
}



export default PostPersona;
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
        "Password": `${persona.password}`,
        "ConfirmPassword":`${persona.confirmPassword}`
      }
    );

    const requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

  fetch("http://cuentasctacte-web-api20220425205158.azurewebsites.net/api/Account/Register", requestOptions)
        .then(response => {
            response.ok ? alert("Guardado con exito") : alert("Error al registrar al usuario")
        })
        .catch(error => console.log(error));
}



export default PostPersona;
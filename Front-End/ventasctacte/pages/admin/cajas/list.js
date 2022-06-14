import React, { Fragment, useCallback, useEffect, useState } from "react";
import { useRouter } from "next/router";
import getCajas from "../../../API/getCajas";
import CajaList from "../../../components/admin/CajaList";
import AdminNavBar from "../../../components/admin/AdminNavBar";

export default function List() {
    const [cajas, setCajas] = useState([]);
    const router = useRouter();



    const handleLoad = useCallback(() => {
        if (sessionStorage.getItem("token")) {
            const token = JSON.parse(sessionStorage.getItem("token"));
            getCajas(token.access_token)
                .then(result => {
                    if (result.ok) {
                        return result.json();

                    } else if (result.status == 401 || result.status == 403) {
                        alert("No cuenta con los privilegios suficientes");
                        router.push("/LogIn");
                    }

                    else {
                        console.log("Error");
                    }
                })
                .then(result => setCajas(result))
                .catch(console.log);
        } else { router.push("/LogIn") }

    });
    const handlePostCaja = (value) => { }
    const handlePutCaja = (id, value) => {
    }



    useEffect(() => {
        handleLoad();
    }, []);


    return (
        <Fragment>
            <AdminNavBar />
            <div style={{marginLeft:"10%", marginRight:"10%", marginTop:"40px"}}>
                <CajaList cajas={cajas}>

                </CajaList>
            </div>


        </Fragment>

    )

}
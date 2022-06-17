import { useRouter } from "next/router";
import { Fragment, useEffect, useState } from "react";
import getStocks from "../../API/getStocks";
import AdminNavbar from "../../components/admin/AdminNavbar";
import StockList from "../../components/admin/StocksList";

const Stock = () => {
    const router = useRouter();
    const [stocks, setStocks] = useState([]);
    const loadData = () => {
        const token = JSON.parse(sessionStorage.getItem("token"));
        console.log(token);
        if (sessionStorage.getItem("token")) {


            getStocks(token.access_token)
                .then(r => {

                    if (r.ok) {
                        r.json().then(setStocks);
                    } else {
                        router.push("/LogIn")
                    }
                })
                .catch(console.log)
        } else {
            console.log("Ups");
            router.push("/LogIn");
        }

    }
    useEffect(() => {
        loadData();
    }, [])

    return (
        <Fragment>
            <AdminNavbar />
            
            <div style={{ marginLeft: "10%", marginRight: "10%", marginTop: "40px" }}>
                <StockList stocks={stocks} />
            </div>
        </Fragment>
    )
}

export default Stock;
import Link from 'next/link'
import styles from '../styles/Home.module.css'
import {useRouter} from 'next/router'

import 'bootstrap/dist/css/bootstrap.min.css';


export default function Home() {
  const Router = useRouter()
  return (
    <Link href='/LogIn' passHref>
      <button>Login</button>
    </Link>
    
  )
}

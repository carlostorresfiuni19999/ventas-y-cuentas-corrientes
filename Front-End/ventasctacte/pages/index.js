import Link from 'next/link'
import styles from '../styles/Home.module.css'

import 'bootstrap/dist/css/bootstrap.min.css';


export default function Home() {
  return (
    <ul>
      <li>
        <Link href="/">
          <a>Home</a>
        </Link>
      </li>
      <li>
        <Link href="/LogIn">
          <a>Login</a>
        </Link>
      </li>
      <li>
        <Link href="/ndp/lista">
          <a>Notas de Pago</a>
        </Link>
      </li>
      
    </ul>
  )
}

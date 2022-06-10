import Link from 'next/link'
import styles from '../styles/Home.module.css'
import {useRouter} from 'next/router'

export default function Home() {
  const Router = useRouter()
  return (
    <Link href='/Login' passHref>
      <button>Login</button>
    </Link>
    
  )
}

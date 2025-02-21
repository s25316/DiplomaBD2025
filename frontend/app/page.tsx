import Image from "next/image";
import  Link  from 'next/link';
import LogIn from "./components/buttons/LogIn";



export default function Home() {
  return (
    <main>
    <LogIn/>
    <h1>MAIN PAGE </h1>
    <Link href="/offers">Offers</Link>
    
    
    </main>
    
  );
}

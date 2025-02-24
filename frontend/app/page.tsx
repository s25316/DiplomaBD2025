import Image from "next/image";
import  Link  from 'next/link';
import LogIn from "./components/buttons/LogIn";
import ThemeToggle from "./components/buttons/ThemeToggle";



export default function Home() {
  return (
      <main>
      <LogIn/>
      <ThemeToggle/>
      <h1>MAIN PAGE </h1>
      <Link href="/offers">Offers</Link>
      
      
      </main>
    
  );
}

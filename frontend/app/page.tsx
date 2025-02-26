import  Link  from 'next/link';
import ThemeToggle from "./components/buttons/ThemeToggle";

export default function Home() {
  return (
      <main>
      <ThemeToggle/>
      <h1>MAIN PAGE </h1>
      <Link href="/offers">Offers</Link>
      
      
      </main>
    
  );
}

import  Link  from 'next/link';

export default function Home() {
  return (
      <main>
      <h1>MAIN PAGE </h1>
      <Link href="/offers">Offers</Link>
      </main>
  );
}

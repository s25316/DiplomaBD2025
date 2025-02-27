import React from 'react'
import Link from 'next/link'

// interface Offer{
//     id: number;
//     name: string;
// }

const Offers = async () => {
    // const res = await fetch('');
    // const offers: Offer[] =await res.json();



  return (
    
    <div> 
        <h1>Offers </h1>
        <ul>
            <Link href="/offers/123" >off</Link>
            {/* offers.map(offer => <li key={offer.id}>offer.name</li> ) */}
        </ul>
    </div>
  )
}

export default Offers
'use client'
import { useRouter } from 'next/navigation'
import React from 'react'

const MainPageButtonCl = () => {
  const router = useRouter();

  const handleClick = () => {

    router.push("/");
  };
  return (
    <div>
        <button onClick={handleClick} >MainPage</button>
    
    </div>
  )
}

export default MainPageButtonCl
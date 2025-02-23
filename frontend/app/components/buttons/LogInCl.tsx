'use client'
import { useRouter } from 'next/navigation'
import React from 'react'

const LogInCl = () => {
    const router = useRouter();

    const handleClick = () => {
        router.push("/users/logIn")
    }
  return (
    <div>
        <button onClick={handleClick} >LogIn</button>
    </div>
  )
}

export default LogInCl
"use client"
import { signIn, signOut, useSession } from 'next-auth/react'
import React from 'react'

const AppBar = () => {
  const { data: session } = useSession();

  return (
    <div>
      {session?.user ? (
        <button onClick={() => signOut()}>Sign Out</button>
      ) : (
        <>
          <button onClick={() => signIn()}>Sign In</button>
          <button onClick={() => { window.location.href = '/register' }}>Sign Up</button>
        </>
      )}
    </div>
  )
}

export default AppBar
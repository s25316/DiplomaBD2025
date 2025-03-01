"use client"
import { signIn, signOut, useSession } from 'next-auth/react'
import React from 'react'
import ThemeToggle from './buttons/ThemeToggle';
import ReturnButton from './buttons/ReturnButton';

const AppBar = () => {
  const { data: session } = useSession();

  return (
    <div>
      
      <button onClick={() => { window.location.href = '/' }}>MainPage</button>
      <ReturnButton/>
      <ThemeToggle/>

      {session?.user ? (
        <>
        <button onClick={() => { window.location.href = '/profile' }}>Profile</button>
        <button onClick={() => signOut()}>Sign Out</button>
        </>
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
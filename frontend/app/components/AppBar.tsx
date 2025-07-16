"use client"
import { signIn, signOut, useSession } from 'next-auth/react'
import React from 'react'
import ThemeToggle from './buttons/ThemeToggle';
import ReturnButton from './buttons/ReturnButton';
import { useRouter } from 'next/navigation';
import Link from 'next/link';

const AppBar = () => {
  const { data: session } = useSession();

  return (
    <div>
      
      <button onClick={() => { window.location.href = '/' }}>MainPage</button>
      <ReturnButton/>
      <ThemeToggle/>
      {session?.user && ( // Pokaż przycisk tylko, jeśli użytkownik jest zalogowany, trzeba bedzie sprawdzac jakoś potem czy ma company czy nie dla dalszych czynności
        <Link href="/recruitments" passHref>
          <button className="px-4 py-2 rounded-md bg-purple-500 text-white hover:bg-purple-600 transition duration-200">
            Recruitments
          </button>
        </Link>
      )}
      {session?.user? (
        <>
        <button onClick={() => { window.location.href = '/profile' }}>Profile</button>
        <button onClick={() => {
          signOut()
          let router = useRouter()
          router.push("/")
          }}>Sign Out</button>
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
"use client"
import { signOut, useSession } from 'next-auth/react'
import React from 'react'
import ThemeToggle from './buttons/ThemeToggle';
import ReturnButton from './buttons/ReturnButton';
import { useRouter } from 'next/navigation';
import Link from 'next/link';

const AppBar = () => {
  const { data: session } = useSession();
  const router = useRouter()

  return (
    <div className="flex justify-between items-center px-4 py-3 bg-white dark:bg-gray-800 shadow-md">
      <div className="flex items-center space-x-4">
        <button onClick={() => { window.location.href = '/' }}>MainPage</button>
        <ReturnButton/>
        <ThemeToggle/>
      </div>
      <div className="flex items-center space-x-4">
        {session?.user && ( // Pokaż przycisk tylko, jeśli użytkownik jest zalogowany, trzeba bedzie sprawdzac jakoś potem czy ma company czy nie dla dalszych czynności
          <Link href="/recruitments" passHref>
            <button className="px-4 py-2 rounded-md bg-purple-500 text-white hover:bg-purple-600 transition duration-200">
              Recruitments
            </button>
          </Link>
        )}
        {session?.user? (
          <>
          <button onClick={() => { window.location.href = '/profile' }} className="px-4 py-2 rounded-md text-gray-800 dark:text-white bg-gray-200 dark:bg-gray-700 hover:bg-gray-300 dark:hover:bg-gray-600 transition duration-200">
            Profile
          </button>
          <button onClick={() => { signOut()
            router.push("/") }} 
            className="px-4 py-2 rounded-md bg-red-500 text-white hover:bg-red-600 transition duration-200">
              Sign Out
            </button>
          </>
        ) : (
          <>
            <button onClick={() => router.push('/login')} className="px-4 py-2 rounded-md bg-green-500 text-white hover:bg-green-600 transition duration-200">
              Sign In
            </button>
            <button onClick={() => { window.location.href = '/register' }}  className="px-4 py-2 rounded-md bg-indigo-500 text-white hover:bg-indigo-600 transition duration-200">
              Sign Up
            </button>
          </>
        )}
      </div>
    </div>
  )
}

export default AppBar
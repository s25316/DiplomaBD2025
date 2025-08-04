'use client'

import { signIn } from 'next-auth/react'
import React, { useState } from 'react'
import { OuterContainer } from '../components/layout/PageContainers'
import { useRouter } from 'next/navigation'

const LoginPage = () => {
    const [login, setLogin] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)
    const router = useRouter()

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setError('')
        setIsLoading(true)

        try {
            const result = await signIn("credentials", {
                redirect: false,
                email: login,
                password: password,
                callbackUrl: '/profile'
            })

            if (!result || result.error) {
                console.error('Sign in error:', result?.error)
                setError('Invalid email or password')
            } else {
                router.push('/profile')
                router.refresh()
            }
        } catch (err) {
            console.error('Login error:', err)
            setError('An error occurred during login')
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <OuterContainer className="w-[500px] p-8">
            <h1 className="text-2xl font-bold mb-6 text-center">Log In</h1>
            <form className='flex flex-col space-y-4' onSubmit={handleSubmit}>
                <div className="space-y-2">
                    <label htmlFor='email' className="block text-sm font-medium">
                        Email:
                    </label>
                    <input
                        id='email'
                        name='email'
                        type="email"
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setLogin(e.target.value)}
                        value={login}
                        disabled={isLoading}
                        required
                    />
                </div>

                <div className="space-y-2">
                    <label htmlFor='password' className="block text-sm font-medium">
                        Password:
                    </label>
                    <input
                        id='password'
                        name='password'
                        type="password"
                        className="w-full p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        onChange={e => setPassword(e.target.value)}
                        value={password}
                        disabled={isLoading}
                        required
                    />
                </div>

                {error && (
                    <div className="text-red-500 text-sm text-center">
                        {error}
                    </div>
                )}

                <button
                    type='submit'
                    className="w-full bg-blue-500 text-white p-2 rounded-md hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed"
                    disabled={isLoading}
                >
                    {isLoading ? 'Logging in...' : 'Log in'}
                </button>
            </form>
        </OuterContainer>
    )
}

export default LoginPage
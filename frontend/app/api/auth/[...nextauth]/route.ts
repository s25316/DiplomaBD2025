import NextAuth, { AuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";

export const authOptions: AuthOptions = {
    providers:[
        CredentialsProvider({
            name: 'Credentials',
            credentials: {
                email: { label: "Email", type: "email" },
                password: { label: "Password", type: "password" }
            },
            async authorize(credentials, req) {
                const res = await fetch("http://localhost:8080/api/User/login", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Access-Control-Allow-Origin": "*",
                    },
                    body: JSON.stringify({
                        login: credentials?.email,
                        password: credentials?.password,
                    }),
                });

                const user = await res.json();

                if(user){
                    return user;
                }
                else{
                    return null;
                }
            },
        })
    ],
}

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST }
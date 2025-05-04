// import NextAuth, { DefaultSession, DefaultUser } from "next-auth";

// declare module "next-auth" {
//   interface User extends DefaultUser {
//     token: string;
//   }

//   interface Session extends DefaultSession {
//     user: User;
//   }
// }

import NextAuth, { DefaultSession, DefaultUser } from "next-auth";

declare module "next-auth" {
  interface User extends DefaultUser {
    token: string;
    refreshToken: string;
    jwtValidTo: string;
    refreshTokenValidTo: string;
  }

  interface Session extends DefaultSession {
    user: User;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken: string;
    refreshToken?: string;
    jwtValidTo?: string;
    refreshTokenValidTo?: string;
  }
}

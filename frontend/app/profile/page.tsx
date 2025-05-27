import React from "react";
import CreateCompanyButton from "@/app/components/buttons/CreateCompanyButton"
import { getServerSession } from "next-auth";
import { authOptions } from "../api/auth/[...nextauth]/route";
import { redirect } from "next/navigation";
import Link from "next/link";

interface CompanyProfile {
  companyId: string,
  name: string,
  description: string,
  websiteUrl?: string;
}

const Profile = async () => {
  const session = await getServerSession(authOptions);

  if (!session?.user) {
    redirect("/api/auth/signin")
  }

  const res = await fetch("http://localhost:8080/api/CompanyUser/companies", {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${session?.user.token}`
    }
  })

  var companies: CompanyProfile[] =  []

  if (res.ok) {
    let tmp = await res.json()
    companies = tmp.items;
  }

  return (
    <div>
      <h1>Profile</h1>
      <p>Logged in as: {session.user.token}</p>
      {companies &&
        <>
          <h2>Companies:</h2>
          <ul>
            {companies.map((value) => 
              <li key={value.companyId} className="border p-3 rounded my-2 max-w-md">
                <Link href={`/companies/${value.companyId}`}><b>Name: {value.name}</b></Link></li>
            )}
          </ul>
        </>
      }
      <CreateCompanyButton />
    </div>
  );
};

export default Profile;

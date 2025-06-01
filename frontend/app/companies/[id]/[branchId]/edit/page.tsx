"use client";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState, useRef } from "react";
import { useSession } from "next-auth/react";
import BranchForm from "@/app/components/forms/BranchForm";

interface Address {
  countryName: string;
  stateName: string;
  cityName: string;
  streetName: string | null;
  houseNumber: string;
  apartmentNumber: string | null;
  postCode: string;
  lon: number;
  lat: number;
}

interface Branch {
  name: string;
  description: string | null;
  address: Address;
}

const EditBranch = () => {
  const { data: session } = useSession();
  const { id, branchId } = useParams();
  const router = useRouter();

  const [branch, setBranch] = useState<Branch | null>(null);
  const [formState, setFormState] = useState<any>(null); // will be passed to <BranchCreateForm />
  const sendData = useRef<any>(null);

  useEffect(() => {
    if (!session?.user?.token) return;

    const fetchBranch = async () => {
      const res = await fetch(`http://localhost:8080/api/CompanyUser/branches/${branchId}`, {
        headers: {
          Authorization: `Bearer ${session.user.token}`,
        },
      });

      if (res.ok) {
        const data = await res.json();
        const b = data.items[0].branch;
        setBranch(b);

        setFormState({
          index: 0,
          name: b.name,
          description: b.description,
          address: { ...b.address },
        });

        sendData.current = {
          index: 0,
          name: b.name,
          description: b.description,
          address: { ...b.address },
        };
      }
    };

    fetchBranch();
  }, [session, branchId]);

  const handleData = (data: any) => {
    sendData.current = data;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
      name: sendData.current.name,
      description: sendData.current.description,
      address: {
        ...sendData.current.address,
      },
    };

    const res = await fetch(`http://localhost:8080/api/CompanyUser/companies/branches/${branchId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user.token}`,
      },
      body: JSON.stringify(payload),
    });

    if (res.ok) {
      alert("Branch updated.");
      router.push(`/companies/${id}/${branchId}`);
    } else {
      alert("Failed to update branch.");
    }
  };

  if (!formState) return <div>Loading...</div>;

  return (
    <div className="flex flex-col gap-4">
      <h1>Edit Branch</h1>
      <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
        <BranchForm index={0} getData={handleData} initialData={formState} />
        <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded">Save Changes</button>
      </form>
    </div>
  );
};

export default EditBranch;

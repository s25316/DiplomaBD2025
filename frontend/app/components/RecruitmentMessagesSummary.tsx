'use client';

import React, { useEffect, useState, useCallback } from 'react';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

interface RecruitmentMessage {
  messageId: string;
  message: string;
  created: string;
  isPersonSend: boolean;
}

interface RecruitmentMessagesSummaryProps {
  processId: string;
}

const RecruitmentMessagesSummary: React.FC<RecruitmentMessagesSummaryProps> = ({ processId }) => {
  const { data: session, status } = useSession();
  const [messages, setMessages] = useState<RecruitmentMessage[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [isExpanded, setIsExpanded] = useState<boolean>(false);
  const [isIndividual, setIsIndividual] = useState<boolean | null>(null);

  const backUrl = process.env.NEXT_PUBLIC_API_URL;

  const fetchMessages = useCallback(async () => {
    if (status !== 'authenticated' || !session?.user?.token) {
      setLoading(false);
      return;
    }

    setLoading(true);
    try {
      const userRes = await fetch(`${backUrl}/api/User`, {
        headers: { Authorization: `Bearer ${session.user.token}` },
      });
      const user = await userRes.json();
      const isUserIndividual = user.personPerspective.isIndividual;
      setIsIndividual(isUserIndividual);

      let apiUrl = `${backUrl}/api/`;
      if (!isUserIndividual) {
        apiUrl += `CompanyUser/recruitments/${processId}/messages?Page=1&ItemsPerPage=2&ascending=true`;
      } else {
        apiUrl += `User/recruitments/${processId}/messages?Page=1&ItemsPerPage=2&ascending=true`;
      }

      const res = await fetch(apiUrl, {
        headers: { Authorization: `Bearer ${session.user.token}` },
        cache: 'no-store',
      });

      if (!res.ok) {
        throw new Error('Failed to fetch messages');
      }

      const data = await res.json();
      setMessages(data.items || []);
    } catch (err) {
      console.error('Error fetching messages:', err);
    } finally {
      setLoading(false);
    }
  }, [session, status, processId]);

  useEffect(() => {
    fetchMessages();
  }, [fetchMessages]);

  if (loading || status === 'loading') {
    return <div className="text-center text-sm py-2 text-gray-500 dark:text-gray-400">Loading messages...</div>;
  }

  const messagesToDisplay = isExpanded ? messages : messages.slice(0, 2);

  return (
    <div className="bg-gray-50 dark:bg-gray-800 p-6 rounded-lg shadow-inner mb-6">
      <h2 className="text-2xl font-bold mb-4 text-gray-800 dark:text-gray-100 flex justify-between items-center">
        <span>Recent Messages</span>
        <button
          onClick={() => setIsExpanded(!isExpanded)}
          className="text-sm font-normal text-blue-600 dark:text-blue-400 hover:underline flex items-center"
        >
          {isExpanded ? (
            <>
              Show less -
            </>
          ) : (
            <>
              Show all +
            </>
          )}
        </button>
      </h2>
      
      {messages.length > 0 ? (
        <ul className="space-y-4">
          {messagesToDisplay.map((msg) => (
            <li key={msg.messageId} className={`p-3 rounded-lg shadow-sm ${msg.isPersonSend !== isIndividual ? 'bg-blue-100 dark:bg-gray-700' : 'bg-gray-100 dark:bg-blue-800'}`}>
              <p className="font-semibold text-sm">
                {msg.isPersonSend !== isIndividual ? !isIndividual ? 'Candidate' : "Company" : 'Me'}:
              </p>
              <p className="mt-1 text-gray-800 dark:text-gray-200">{msg.message}</p>
              <p className="text-xs text-gray-500 dark:text-gray-400 mt-2">
                {new Date(msg.created).toLocaleString()}
              </p>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-gray-600 dark:text-gray-400">No messages for this recruitment yet.</p>
      )}

      <div className="flex justify-center mt-6">
        <Link
          href={`/recruitments/${processId}/messages`}
          className="text-blue-600 dark:text-blue-400 hover:underline font-semibold"
        >
          View Full Message History →
        </Link>
      </div>
    </div>
  );
};

export default RecruitmentMessagesSummary;